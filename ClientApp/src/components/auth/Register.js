import React from 'react'
import { Redirect } from 'react-router-dom'
import OktaAuth from '@okta/okta-auth-js'
import { withAuth } from '@okta/okta-react'
import config from '../../app.config'

export default withAuth(class RegisterPage extends React.Component {

    constructor(props) {
        super(props)
        this.state = {
            firstName: "",
            lastName: "",
            email: "",
            password: "",
            sessionToken: null,
            registered: false
        }

        this.oktaAuth = new OktaAuth({ url: config.url })
        this.checkAuthentication = this.checkAuthentication.bind(this)
        this.checkAuthentication()

        this.handleSubmit = this.handleSubmit.bind(this)
        this.handleFirstNameChanged = this.handleFirstNameChanged.bind(this)
        this.handleLastNameChanged = this.handleLastNameChanged.bind(this)
        this.handleEmailChange = this.handleEmailChange.bind(this)
        this.handlePasswordChanged = this.handlePasswordChanged.bind(this)
    }

    async checkAuthentication() {
        const sessionToken = await this.props.auth.getIdToken();
        console.log(sessionToken)
        if (sessionToken) {
            this.setState({ sessionToken })
        }
    }

    componentDidUpdate() {
        this.checkAuthentication()
    }

    handleFirstNameChanged(e) {
        this.setState({ fistname: e.target.value })
    }

    handleLastNameChanged(e) {
        this.setState({ lastName: e.target.value })
    }

    handleEmailChange(e) {
        this.setState({ email: e.target.value })
    }

    handlePasswordChanged(e) {
        this.setState({ password: e.target.value })
    }

    handleSubmit(e) {
        e.preventDefault()
        fetch('/api/register', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(this.state)
        }).then(user => {
            this.setState({ registered: true })
        }).catch(err => console.log)
    }

    render() {
        if (this.sessionToken) {
            this.props.auth.redirect({ sessionToken: this.state.seesionToken })
            return null
        }

        if (this.state.registered == true) {
            return <Redirect to="/login" />
        }
        return (
            <h1>Welcome to the register page!</h1>
        )
    }

})


