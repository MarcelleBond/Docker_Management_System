import React from 'react';
import OkatAuth from '@okta/okta-auth-js';
import { withAuth } from "@okta/okta-react";

export default withAuth(class LoginPage extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            sessionToke: null,
            error: null,
            username: '',
            password: ''
        }

        this.okatAuth = new OkatAuth({ url: props.baseUrl })
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleUsernameChange = this.handleUsernameChange.bind(this)
        this.handlePasswordChange = this.handlePasswordChange.bind(this)
    }

    handleSubmit(e) {
        e.preventDefault();
        this.okatAuth.signIn({
            username: this.state.username,
            password: this.state.password
        })
            .then(res => this.setState({
                sessionToke: res.sessionToke
            }))
            .catch(err => {
                this.setState({ error: err.message })
                console.log(err.statusCode + "error", err)
            })
    }

    handleUsernameChange(e) {
        this.setState({ username: e.target.value })
    }

    handlePasswordChange(e) {
        this.setState({ password: e.tartget.value })
    }

    render() {
        if (this.state.sessionToke) {
            this.props.auth.redirect({ sessionToke: this.state.sessionToke })
            return null;
        }
        return (
            <h1>Welcome to the login page!</h1>
        )
    }

})