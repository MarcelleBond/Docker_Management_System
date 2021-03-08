import React, { Component } from 'react';
import { SecureRoute, LoginCallback, Security} from "@okta/okta-react";
import { Route, withRouter  } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import FetchData from './components/FetchData';
import { Counter } from './components/Counter';
import RegisterPage from './components/auth/Register';
import LoginPage from './components/auth/Login';
import ProfilePage from './components/auth/Profile';
import config from './app.config';
import { OktaAuth } from '@okta/okta-auth-js';


import './custom.css'

export default withRouter (class App extends Component {

  constructor(props) {
    super(props);
    this.onAuthRequired = this.onAuthRequired.bind(this);

    this.oktaAuth = new OktaAuth({issuer:config.issuer,
      clientId:config.client_id,
      redirectUri:config.redirect_uri,
      pkce: config.pkce})
  }

  onAuthRequired() {
    this.props.history.push('/login');
  }

  render () {
    return (
      <Security oktaAuth={this.oktaAuth}
                onAuthRequired={this.onAuthRequired}>
        <Layout>
        <Route exact path='/' component={Home} />
        <SecureRoute path='/counter' component={Counter} />
        <SecureRoute path='/fetch-data' component={FetchData} />
        <SecureRoute path="/profile" component={ProfilePage} />
        <Route path="/login" render={() => <LoginPage baseUrl={config.url} />} />
        <Route path="/login/callback" component={LoginCallback} />
        <Route path="/register" component={RegisterPage} />
        </Layout>
      </Security>
    );
  }
})
