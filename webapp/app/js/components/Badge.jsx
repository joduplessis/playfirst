import React, { Component } from 'react'
import ReactDOM from 'react-dom'
import moment from 'moment'
import $ from 'jquery'
import jQuery from 'jquery'
import request from 'es6-request'
import TinyMCE from 'react-tinymce'
import { Alert, Button } from 'react-bootstrap'
import Range from 'react-range'

export default class Badge extends Component {
  constructor (props) {
    super(props)

    this.state = {
      id: 0,
      image: '',
      title: '',
      description: '',
      node: 0,
      nodes: [],
      points: 0,
      alertVisible: false,
      file_upload_progress: 0
    }

    this.handleSubmit = this.handleSubmit.bind(this)
    this.handleImageChange = this.handleImageChange.bind(this)
    this.handleTitleChange = this.handleTitleChange.bind(this)
    this.handleDescriptionChange = this.handleDescriptionChange.bind(this)
    this.handleNodeChange = this.handleNodeChange.bind(this)
    this.handleAlertDismiss = this.handleAlertDismiss.bind(this)
    this.handleRangeChange = this.handleRangeChange.bind(this)
  }

  handleRangeChange(event) {
    this.setState({points: event.target.value, alertVisible: false})
  }

  handleNodeChange(event) {
    this.setState({node: event.target.value, alertVisible: false})
  }

  handleDescriptionChange(event) {
    this.setState({description: event.target.value, alertVisible: false})
  }

  handleTitleChange(event) {
    this.setState({title: event.target.value, alertVisible: false})
  }

  handleSubmit(event) {
    event.preventDefault()

    // Send a sanitized JSON object ready for CodeIgnitor
    var sanitizedJsonObjectForApi = {
      id: this.state.id,
      image: this.state.image,
      title: this.state.title,
      description: this.state.description,
      node: this.state.node,
      points: this.state.points,
    }

    // Make the request
    request.post(window.base_url+"badges/update_badge")
      .query(sanitizedJsonObjectForApi)
      .then((response) => {
          this.setState({alertVisible: true})
      })
  }

  handleImageChange(event) {
    event.preventDefault()

    this.handleFileUpload(this.refs.file_image.files)
  }

  handleFileUpload(files) {
    var context = this
    var progressBar = context.refs.progress

    if (files.length!=0) {
      var fileName = files[0].name
      var request = new XMLHttpRequest()
      var formData = new FormData()

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
        context.setState({image: event.currentTarget.responseText, file_upload_progress: 0})
      }

      // Send request
      request.send(formData)
    }
  }

  componentDidMount() {
    fetch(window.base_url+'api/nodes_get_all')
      .then(response => response.json())
      .then(data => this.populateNodes(data))
      .catch(err => console.error(this.props.url, err.toString()))
  }

  populateNodes(data) {
    this.setState({nodes: data})

    fetch(window.base_url+'api/badges_get/'+this.props.params.id)
      .then(response => response.json())
      .then(data => this.populateComponentState(data))
      .catch(err => console.error(this.props.url, err.toString()))
  }

  populateComponentState(data) {
    if (data.id!=null) this.setState({id: data.id})
    if (data.image!=null) this.setState({image: data.image})
    if (data.title!=null) this.setState({title: data.title})
    if (data.description!=null) this.setState({description: data.description})
    if (data.node!=null) this.setState({node: data.node})
    if (data.points!=null) this.setState({points: data.points})
  }

  handleAlertDismiss() {
    this.setState({alertVisible: false})
  }

  render() {
    var progressStyles = {
      width: this.state.file_upload_progress+'0%'
    }

    var image_url = window.base_url+''+this.state.image

    var delete_url = window.base_url+'badges/badge_delete/'+this.state.id

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
        					<h1>Badge</h1>

        					<form onSubmit={this.handleSubmit}>
        					  <div className="form-group">
        					    <label>Title</label>
        					    <input type="text" className="form-control form-control" id="exampleInputEmail1" aria-describedby="emailHelp" placeholder="Title"  value={this.state.title} onChange={this.handleTitleChange}/>
        					  </div>

                    <div className="form-group">
        					    <label>Description</label>
          					  <textarea className="form-control" id="exampleTextarea" rows="10" value={this.state.description} onChange={this.handleDescriptionChange}></textarea>
        					  </div>

                    <div className="form-group">
                      <label>Node</label>
                      <select className="form-control" value={this.state.node} onChange={this.handleNodeChange}>
                        {this.state.nodes.map((node, i) =>
                            <option value={node.id} key={i}>{node.title}</option>
                        )}
                      </select>
                    </div>

          					<div className="form-group">
                      <label>Image</label><br/>
                      <input type="file" name="file_image" ref="file_image"/><br/>
                      <button onClick={this.handleImageChange} className="upload-button">Update image now</button>
                    </div>

                    <div className="form-group">
                      <label className="mr-20">Points ({this.state.points})</label>
                      <Range
                        className='slider'
                        onChange={this.handleRangeChange}
                        type='range'
                        value={this.state.points}
                        min={0}
                        max={100} />
                    </div>

        					  <button type="submit" className="btn btn-primary">Save</button>
        					</form>
        				</div>

        				<a href={delete_url} className="footer"><span className="fa fa-trash"></span> Delete</a>
        			</div>

        			<div className="col-6">
        				<div className="inner pt-10 pb-10 pl-10 pr-10">
        					<h1>Image</h1>
        					<label>Mixed content can consist of rich text</label>
                  <img src={image_url} width="100%"/>
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
