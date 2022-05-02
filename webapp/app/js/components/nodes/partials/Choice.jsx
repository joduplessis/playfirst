import React, { Component } from 'react'
import ReactDOM from 'react-dom'
import $ from 'jquery'
import jQuery from 'jquery'

export default class Choice extends Component {
  constructor (props) {
    super(props)

    this.state = {
      id: this.props.id,
      title: this.props.title,
      correct: this.props.correct,
    }

    this.handleTitleChange = this.handleTitleChange.bind(this)
    this.handleCorrectChange = this.handleCorrectChange.bind(this)
    this.handleDeleteClick = this.handleDeleteClick.bind(this)
  }

  handleTitleChange(event) {
    this.setState({title: event.target.value}, function() {
      this.props.handleChoiceChange(this.state)
    })
  }

  handleCorrectChange(event) {
    this.setState({correct: event.target.checked}, function() {
      this.props.handleChoiceChange(this.state)
    })
  }

  handleDeleteClick(event) {
    this.props.handleChoiceDelete(this.state.id)
  }

  componentDidMount() {
  }

  render() {
    return (
      <div className="row mb-10">
        <div className="col-8">
          <input type="text" className="form-control form-control" aria-describedby="emailHelp" placeholder="Title" value={this.state.title} onChange={this.handleTitleChange}/>
        </div>
        <div className="col-2 pt-10">
			    <input type="checkbox" className="form-control form-control" aria-describedby="emailHelp" placeholder="Title" checked={this.state.correct} onChange={this.handleCorrectChange}/>
        </div>
        <div className="col-2">
			    <button type="button" className="btn btn-primary" onClick={this.handleDeleteClick}><span className="fa fa-times"></span></button>
        </div>
      </div>
    )
  }
}
