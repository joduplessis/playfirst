import React, { Component } from 'react'
import ReactDOM from 'react-dom'
import moment from 'moment'
import $ from 'jquery'
import jQuery from 'jquery'
import request from 'es6-request'
import TinyMCE from 'react-tinymce'
import { Alert, Button } from 'react-bootstrap'

export default class Guide extends Component {
  constructor (props) {
    super(props)

    this.state = {
      id: 0,
      text: '',
      node: 0,
      nodes: [],
      alertVisible: false,
    }

    this.handleSubmit = this.handleSubmit.bind(this)
    this.handleTextChange = this.handleTextChange.bind(this)
    this.handleNodeChange = this.handleNodeChange.bind(this)
    this.handleAlertDismiss = this.handleAlertDismiss.bind(this)
  }

  handleNodeChange(event) {
    this.setState({node: event.target.value, alertVisible: false})
  }

  handleTextChange(event) {
    this.setState({text: event.target.value, alertVisible: false})
  }

  handleSubmit(event) {
    event.preventDefault()

    // Send a sanitized JSON object ready for CodeIgnitor
    var sanitizedJsonObjectForApi = {
      id: this.state.id,
      text: this.state.text,
      node: this.state.node
    }

    // Make the request
    request.get(window.base_url+"guides/guide_update")
      .query(sanitizedJsonObjectForApi)
      .then((response) => {
          this.setState({alertVisible: true})
      })
  }


  componentDidMount() {
    fetch(window.base_url+'api/nodes_get_all')
      .then(response => response.json())
      .then(data => this.populateNodes(data))
      .catch(err => console.error(this.props.url, err.toString()))
  }

  populateNodes(data) {
    this.setState({nodes: data})

    fetch(window.base_url+'api/guides_get/'+this.props.params.id)
      .then(response => response.json())
      .then(data => this.populateComponentState(data))
      .catch(err => console.error(this.props.url, err.toString()))
  }

  populateComponentState(data) {
    if (data.id!=null) this.setState({id: data.id})
    if (data.text!=null) this.setState({text: data.text})
    if (data.node!=null) this.setState({node: data.node})
  }

  handleAlertDismiss() {
    this.setState({alertVisible: false})
  }

  render() {
    var delete_url = window.base_url+'guides/guide_delete/'+this.state.id

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
        			<div className="col-12">
        				<div className="inner pt-10 pb-10 pl-10 pr-10">
        					<h1>Guide</h1>

        					<form onSubmit={this.handleSubmit}>
        					  <div className="form-group">
        					    <label>Text</label>
        					    <input type="text" className="form-control form-control" id="exampleInputEmail1" aria-describedby="emailHelp" placeholder="Title"  value={this.state.text} onChange={this.handleTextChange}/>
        					  </div>

                    <div className="form-group">
                      <label>Node</label>
                      <select className="form-control" value={this.state.node} onChange={this.handleNodeChange}>
                        <option value='0'>General</option>
                        {this.state.nodes.map((node, i) =>
                            <option value={node.id} key={i}>{node.title}</option>
                        )}
                      </select>
                    </div>

        					  <button type="submit" className="btn btn-primary">Save</button>
        					</form>
        				</div>

        				<a href={delete_url} className="footer"><span className="fa fa-trash"></span> Delete</a>
        			</div>
        		</div>
        	</div>
          <div className="col-1"></div>
        </div>
      </div>
    )
  }
}
