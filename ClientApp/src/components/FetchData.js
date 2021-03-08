import React, { Component } from 'react';
import { withOktaAuth } from "@okta/okta-react";


export default withOktaAuth(class FetchData extends Component {

  constructor(props) {
    super(props);
    this.state = { forecasts: [], loading: true };
  }

  componentDidMount() {
    this.populateWeatherData();
  }

  static renderForecastsTable(forecasts) {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Date</th>
            <th>Temp. (C)</th>
            <th>Temp. (F)</th>
            <th>Summary</th>
          </tr>
        </thead>
        <tbody>
          {forecasts.map(forecast =>
            <tr key={forecast.date}>
              <td>{forecast.date}</td>
              <td>{forecast.temperatureC}</td>
              <td>{forecast.temperatureF}</td>
              <td>{forecast.summary}</td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : FetchData.renderForecastsTable(this.state.forecasts);

    return (
      <div>
        <h1 id="tabelLabel" >Weather forecast</h1>
        <p>This component demonstrates fetching data from the server.</p>
        {contents}
      </div>
    );
  }

  async populateWeatherData() {
    console.log("accessToken", this.props.authState.accessToken.accessToken)
    const response = await fetch('weatherforecast', {
      headers: {
        'content-type': 'application/json',
        Authorization: 'Bearer ' + this.props.authState.accessToken.accessToken
      }
    });
    console.log(response);
    const data = await response.json();
    this.setState({ forecasts: data, loading: false });
  }
})
