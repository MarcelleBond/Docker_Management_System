import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { FetchData } from './components/FetchData';
import { Counter } from './components/Counter';
import RegisterPage from './components/auth/Register';
import LoginPage from './components/auth/Login';
import ProfilePage from './components/auth/Profile';
import config from './app.config';
import { SecureRoute, ImplicitCallback  } from "@okta/okta-react";

import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/counter' component={Counter} />
        <Route path='/fetch-data' component={FetchData} />
        <SecureRoute path="/profile" component={ProfilePage} />
        <Route path="/login" render={() => <LoginPage baseUrl={config.url} />} />
        <Route path="/implicit/callback" component={ImplicitCallback} />
        <Route path="/register" component={RegisterPage} />
      </Layout>
    );
  }
}
