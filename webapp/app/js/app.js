import React from 'react';
import ReactDOM from 'react-dom';
import Mixed from './components/nodes/Mixed.jsx';
import Choices from './components/nodes/Choices.jsx';
import Wind from './components/nodes/Wind.jsx';
import Video from './components/nodes/Video.jsx';
import Images from './components/nodes/Images.jsx';
import Badge from './components/Badge.jsx';
import Guide from './components/Guide.jsx';
import { Router, Route, hashHistory, browserHistory } from 'react-router'

import '../scss/app.scss';

ReactDOM.render((
  <Router history={browserHistory} path="/">
    <Route path="paths/path_node/mixed/:id" component={Mixed} />
    <Route path="paths/path_node/choices/:id" component={Choices} />
    <Route path="paths/path_node/video/:id" component={Video} />
    <Route path="paths/path_node/images/:id" component={Images} />
    <Route path="paths/path_node/wind/:id" component={Wind} />
    <Route path="badges/badge/:id" component={Badge} />
    <Route path="guides/guide/:id" component={Guide} />
    <Route path="*" />
  </Router>
), document.getElementById('app'))
