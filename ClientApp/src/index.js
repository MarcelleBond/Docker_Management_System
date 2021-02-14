import 'bootstrap/dist/css/bootstrap.css';
import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter } from 'react-router-dom';
import App from './App';
import registerServiceWorker from './registerServiceWorker';
import { Security } from '@okta/okta-react';
import config from './app.config';

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
const rootElement = document.getElementById('root');
const onAuthRequired = ({ history }) => history.push('/login');

ReactDOM.render(
  <BrowserRouter basename={baseUrl}>
    <Security issuer={config.issuer}
      client_id={config.client_id}
      redirect_uri={config.redirect_uri}
      onAuthRequired={onAuthRequired}>
      <App />
    </Security >
  </BrowserRouter>,
  rootElement);

registerServiceWorker();

