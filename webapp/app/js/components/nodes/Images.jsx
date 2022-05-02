import React, { Component } from 'react'
import ReactDOM from 'react-dom'
import moment from 'moment'
import $ from 'jquery'
import jQuery from 'jquery'
import request from 'es6-request'
import TinyMCE from 'react-tinymce';
import { Alert, Button } from 'react-bootstrap';
import Image from './partials/Image.jsx'

export default class Images extends Component {
  constructor (props) {
    super(props)

    this.state = {
      id: 0,
      path: 0,
      type: '',
      title: 'Node title',
      content: '',
      points: 0,
      alertVisible: false,
      file_upload_progress: 0,
      images: []
    }

    this.handleSubmit = this.handleSubmit.bind(this)
    this.handleContentChange = this.handleContentChange.bind(this)
    this.handleTitleChange = this.handleTitleChange.bind(this)
    this.handlePointsChange = this.handlePointsChange.bind(this)
    this.handleAlertDismiss = this.handleAlertDismiss.bind(this)
    this.handleImageChange = this.handleImageChange.bind(this)
    this.handleImageDelete = this.handleImageDelete.bind(this)
  }

  handlePointsChange(event) {
    this.setState({points: event.target.value, alertVisible: false})
  }

  handleTitleChange(event) {
    this.setState({title: event.target.value, alertVisible: false})
  }

  handleContentChange(event) {
    this.setState({content: event.target.getContent(), alertVisible: false})
  }

  handleSubmit(event) {
    event.preventDefault()

    var images = []

    this.state.images.map((image) => {
      images.push(image.url)
    })


    // Send a sanitized JSON object ready for CodeIgnitor
    var sanitizedJsonObjectForApi = {
      id: this.state.id,
      title: this.state.title,
      content: images.join(","),
      points: this.state.points
    }

    // Make the request
    request.post(window.base_url+"paths/update_node")
      .query(sanitizedJsonObjectForApi)
      .then((response) => {
          this.setState({alertVisible: true})
      })
  }

  handleImageChange(event) {
    event.preventDefault()

    this.handleFileUpload(this.refs.file_image.files)
  }

  handleImageDelete(id) {
    var updatedImages = []

    this.state.images.map((image) => {
      if (image.id != id) {
        updatedImages.push(image)
      }
    })

    this.setState({images: updatedImages})
  }

  handleFileUpload(files) {
    var context = this
    var progressBar = context.refs.progress

    if (files.length!=0) {
      var fileName = files[0].name
      var request = new XMLHttpRequest()
      var formData = new FormData()
      var context = this

      // Add form values
      formData.append("file", files[0])
      formData.append("name", fileName)

      // Set up the request type
      request.open("POST", window.base_url+"api/file_upload")

      // Keep tabs on the progress
      request.upload.onprogress = function(event) {
        context.setState({file_upload_progress: 100 * (event.loaded / event.total)})
      }

      request.onload = function(event) {
        var url = event.currentTarget.responseText
        var updatedImages = context.state.images

        updatedImages.push({id: updatedImages.length+1, url: url})
        context.setState({images: updatedImages, file_upload_progress: 0})
      }

      // Send request
      request.send(formData)
    }
  }

  componentDidMount() {
    fetch(window.base_url+'/api/nodes/get/'+this.props.params.id)
      .then(response => response.json())
      .then(data => this.populateComponentState(data))
      .catch(err => console.error(this.props.url, err.toString()))
  }

  populateComponentState(data) {
    if (data.id!=null) this.setState({id: data.id})
    if (data.path!=null) this.setState({path: data.path})
    if (data.type!=null) this.setState({type: data.type})
    if (data.title!=null) this.setState({title: data.title})
    if (data.points!=null) this.setState({points: data.points})
    if (data.content!=null) {
      var content = data.content.split(",")
      var images = []

      content.map((image, i) => {
        images.push({id: i, url: image})
      })

      this.setState({content: data.content, images: images})
    }
  }

  handleAlertDismiss() {
    this.setState({alertVisible: false})
  }

  render() {
    var progressStyles = {
      width: this.state.file_upload_progress+'0%'
    }

    var delete_url = window.base_url+'paths/delete_node/'+this.state.id

    return (
      <div>
        <div className="row grid">
        	<div className="col-1"></div>
        	<div className="col-10">

            <div className="file-upload-progress" id="file-upload-progress" style={progressStyles}> &nbsp; </div>

            {this.state.alertVisible &&
              <Alert bsStyle="info" onDismiss={this.handleAlertDismiss}>
                <strong>Success!</strong> Your node has been saved!
              </Alert>
            }

        		<div className="row">
        			<div className="col-6">
        				<div className="inner pt-10 pb-10 pl-10 pr-10">
        					<h1>Node</h1>

        					<form onSubmit={this.handleSubmit}>
        					  <div className="form-group">
        					    <label>Title</label>
        					    <input type="text" className="form-control form-control" id="exampleInputEmail1" aria-describedby="emailHelp" placeholder="Title"  value={this.state.title} onChange={this.handleTitleChange}/>
        					  </div>

          					<div className="form-group">
                      <label>Image</label><br/>
                      <input type="file" name="file_image" ref="file_image"/><br/>
                      <button onClick={this.handleImageChange} className="upload-button">Update image now</button>
                    </div>

                    <div className="form-group">
                      <label>Points</label>
                      <input type="text" className="form-control" id="exampleInputEmail1" aria-describedby="emailHelp" placeholder="Title"  value={this.state.points} onChange={this.handlePointsChange}/>
                    </div>

        					  <button type="submit" className="btn btn-primary">Save</button>
        					</form>
        				</div>

        				<a href={delete_url} className="footer"><span className="fa fa-trash"></span> Delete</a>
        			</div>

        			<div className="col-6">
        				<div className="inner pt-10 pb-10 pl-10 pr-10">
        					<h1>Images</h1>
        					<label>Select the images to be used as a slideshow.</label>
                  <ul className="list-group">
                    {this.state.images.map((object) => {
                      return <Image key={object.id} url={object.url} id={object.id} handleImageDelete={this.handleImageDelete}/>
                    })}
                  </ul>
        				</div>
        			</div>
        		</div>
        	</div>
          <div className="col-1"></div>
        </div>
      </div>
    )
  }
}
