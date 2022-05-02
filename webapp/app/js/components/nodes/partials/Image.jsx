import React, { Component } from 'react'
import ReactDOM from 'react-dom'
import $ from 'jquery'
import jQuery from 'jquery'

export default class Image extends Component {
  constructor (props) {
    super(props)

    this.state = {
      id: this.props.id,
      url: this.props.url
    }

    this.handleDeleteClick = this.handleDeleteClick.bind(this)
  }

  handleDeleteClick(event) {
    this.props.handleImageDelete(this.state.id)
  }

  componentDidMount() {
  }

  render() {
    var url = window.base_url+"image/thumbnail?image="+window.base_url+""+this.state.url

    return (
      <li className="list-group-item justify-content-between">
        <img src={url} height="50"/>
        <button type="button" className="btn btn-primary pull-right" onClick={this.handleDeleteClick}><span className="fa fa-times"></span></button>
      </li>
    )
  }
}
