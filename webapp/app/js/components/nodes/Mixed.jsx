import React, { Component } from 'react'
import ReactDOM from 'react-dom'
import moment from 'moment'
import $ from 'jquery'
import jQuery from 'jquery'
import request from 'es6-request'
import TinyMCE from 'react-tinymce';
import { Alert, Button } from 'react-bootstrap';
import ReactQuill from 'react-quill';

export default class Mixed extends Component {
  constructor (props) {
    super(props)

    this.state = {
      id: 0,
      path: 0,
      type: '',
      title: 'Node title',
      content: '',
      points: 0,
      alertVisible: false
    }

    this.handleSubmit = this.handleSubmit.bind(this)
    this.handleContentChange = this.handleContentChange.bind(this)
    this.handleTitleChange = this.handleTitleChange.bind(this)
    this.handlePointsChange = this.handlePointsChange.bind(this)
    this.handleAlertDismiss = this.handleAlertDismiss.bind(this)
  }

  handlePointsChange(event) {
    this.setState({points: event.target.value, alertVisible: false})
  }

  handleTitleChange(event) {
    this.setState({title: event.target.value, alertVisible: false})
  }

  handleContentChange(text) {
    this.setState({content: text, alertVisible: false})
  }

  getInitialState() {
    return {
      text: ''
    }
  }

  handleSubmit(event) {
    event.preventDefault()

    // Send a sanitized JSON object ready for CodeIgnitor
    var sanitizedJsonObjectForApi = {
      id: this.state.id,
      title: this.state.title,
      content: this.state.content,
      points: this.state.points
    }

    // Make the request
    request.post(window.base_url+"paths/update_node")
      .query(sanitizedJsonObjectForApi)
      .then((response) => {
          this.setState({alertVisible: true})
      })
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
    if (data.content!=null) this.setState({content: data.content})
    if (data.points!=null) this.setState({points: data.points})
  }

  handleAlertDismiss() {
    this.setState({alertVisible: false})
  }

  render() {
    var delete_url = window.base_url+'paths/delete_node/'+this.state.id

    return (
      <div>
        <div className="row grid">
        	<div className="col-1"></div>
        	<div className="col-10">

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
        					<h1>Mixed content</h1>
        					<label>Mixed content can consist of rich text</label>

                  <ReactQuill
                    value={this.state.content}
                    onChange={this.handleContentChange}
                    theme="snow"
                    modules={{
                      toolbar: [
                        [{ 'header': [1, 2, false] }],
                        ['bold', 'italic', 'underline','strike', 'blockquote'],
                        [{'list': 'ordered'}, {'list': 'bullet'}, {'indent': '-1'}, {'indent': '+1'}],
                        ['link', 'image'],
                        ['clean']
                      ],
                    }}
                    formats={[
                      'header',
                      'bold', 'italic', 'underline', 'strike', 'blockquote',
                      'list', 'bullet', 'indent',
                      'link', 'image'
                    ]}
                    />
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
