import React, { Component } from 'react'
import { withOktaAuth } from '@okta/okta-react'

export default withOktaAuth(class LoginPage extends Component {
	constructor(props) {
		super(props)
		this.state = {
			sessionToken: null,
			username: '',
			password: ''
		}

		this.checkAuthentication = this.checkAuthentication.bind(this)
		this.handleSubmit = this.handleSubmit.bind(this)
		this.handleUsernameChange = this.handleUsernameChange.bind(this)
		this.handlePasswordChange = this.handlePasswordChange.bind(this)
		this.checkAuthentication()
		console.log("I made it in the constructor")
	}

	componentDidMount() {
		this.checkAuthentication();
	}

	handleSubmit(e) {
		e.preventDefault()
		console.log("Hello")
		this.props.oktaAuth.signIn({
			username: this.state.username,
			password: this.state.password
		})
			.then(res => {
				const sessionToken = res.sessionToken
				this.setState(
					{ sessionToken },
					// sessionToken is a one-use token, so make sure this is only called once
					() => this.props.oktaAuth.signInWithRedirect({ sessionToken })
				)
			})
			.catch(err => console.log('Found an error', err.message))
	}

	checkAuthentication() {
		const authenticated = this.props.authState.isAuthenticated;
		if (authenticated) {
			window.history.back();
		}
	}

	handleUsernameChange(e) {
		this.setState({ username: e.target.value })
	}

	handlePasswordChange(e) {
		this.setState({ password: e.target.value })
	}

	render() {
		// if (this.state.sessionToken) {
		//   // Hide form while sessionToken is converted into id/access tokens
		//   console.log("made it here")
		//   this.checkAuthentication()
		//   return null
		// }

		if (this.props.authState.isPending) {
			return <div>Loading...</div>;
		}

		return (
			<form onSubmit={this.handleSubmit}>
				<label>
					Username:
					<input
						id="username" type="text"
						value={this.state.username}
						onChange={this.handleUsernameChange} />
				</label>
				<label>
					Password:
					<input
						id="password" type="password"
						value={this.state.password}
						onChange={this.handlePasswordChange} />
				</label>
				<input id="submit" type="submit" value="Submit" />
			</form>
		)
	}
})