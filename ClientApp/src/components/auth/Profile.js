import React from 'react';
import { withOktaAuth } from "@okta/okta-react";


export default withOktaAuth(class ProfilePage extends React.Component {
    constructor(props) {
        super(props);
        this.state = { user: null }
        this.getCurrentUser = this.getCurrentUser.bind(this)
    }

    async getCurrentUser() {
        this.props.oktaAuth.getUser().then(user => {
            this.setState({ user })
            console.log(this.state.user);
        })
        const response = await fetch('api/userprofile/updatepassword', {
            method: "Post",
            headers: {
                'content-type': 'application/json',
                Authorization: 'Bearer ' + this.props.authState.accessToken.accessToken
            },
            body: JSON.stringify({Password: "new password"})
        });
        console.log("This is the response from the back end :", response)
    }

    componentDidMount() {
        this.getCurrentUser();
    }

    render() {
        if (!this.state.user) {
            return null
        }
        return (<section className="user-profile">
            <h1>{this.state.user.name}'s Submitted Sessions</h1>
        </section>)
    }
})