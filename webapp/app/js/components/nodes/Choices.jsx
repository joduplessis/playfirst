import React, { Component } from 'react'
import ReactDOM from 'react-dom'
import moment from 'moment'
import Choice from './partials/Choice.jsx'
import $ from 'jquery'
import jQuery from 'jquery'
import request from 'es6-request'
import TinyMCE from 'react-tinymce';
import { Alert, Button } from 'react-bootstrap';

export default class Choices extends Component {
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
      choices: []
    }

    this.handleSubmit = this.handleSubmit.bind(this)
    this.handleContentChange = this.handleContentChange.bind(this)
    this.handleTitleChange = this.handleTitleChange.bind(this)
    this.handlePointsChange = this.handlePointsChange.bind(this)
    this.handleChoiceChange = this.handleChoiceChange.bind(this)
    this.handleChoiceDelete = this.handleChoiceDelete.bind(this)
    this.handleChoiceAdd = this.handleChoiceAdd.bind(this)
    this.handleAlertDismiss = this.handleAlertDismiss.bind(this)
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

  handleChoiceChange(event) {
    var updatedChoices = this.state.choices

    updatedChoices.map((choice) => {
      if (choice.id == event.id) {
        choice.title = event.title
        choice.correct = event.correct
      }
    })

    this.setState({choices: updatedChoices})
  }

  handleChoiceDelete(id) {
    var updatedChoices = []

    this.state.choices.map((choice) => {
      if (choice.id != id) {
        updatedChoices.push(choice)
      }
    })

    this.setState({choices: updatedChoices})
  }

  handleChoiceAdd() {
    var updatedChoices = this.state.choices
    updatedChoices.push({id: updatedChoices.length+1, title: 'New mutliple choice', correct: false})
    this.setState({choices: updatedChoices})
  }

  handleSubmit(event) {
    event.preventDefault()

    var choices = []

    this.state.choices.map((choice) => {
      choices.push(choice.title+"|"+choice.correct)
    })

    // Send a sanitized JSON object ready for CodeIgnitor
    var sanitizedJsonObjectForApi = {
      id: this.state.id,
      title: this.state.title,
      content: choices.join("_"),
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
    if (data.points!=null) this.setState({points: data.points})
    if (data.content!=null) {
      var content = data.content.split("_")
      var choices = []

      content.map((choice, i) => {
        choices.push({id: i, title: choice.split('|')[0], correct: JSON.parse(choice.split('|')[1])})
      })

      this.setState({content: data.content, choices: choices})
    }
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
        			<div className="col-4">
        				<div className="inner pt-10 pb-10 pl-10 pr-10">
        					<h1>Node</h1>

        					<form onSubmit={this.handleSubmit}>
        					  <div className="form-group">
        					    <label>Title</label>
                      <textarea onChange={this.handleTitleChange} value={this.state.title} className="form-control"/>
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

        			<div className="col-8">
        				<div className="inner pt-10 pb-10 pl-10 pr-10">
        					<h1>Choices</h1>
        					<label>Multiple choices, remember to select which one is correct.</label>

                  {this.state.choices.map((object) => {
                      return <Choice key={object.id} id={object.id} title={object.title} correct={object.correct} handleChoiceChange={this.handleChoiceChange} handleChoiceDelete={this.handleChoiceDelete}/>
                  })}
        				</div>

        				<a href="" className="footer" onClick={this.handleChoiceAdd} data-toggle="modal" data-target=".modal"><span className="fa fa-plus"></span> Add new choice</a>
        			</div>
        		</div>
        	</div>
          <div className="col-1"></div>
        </div>
      </div>
    )
  }
}
