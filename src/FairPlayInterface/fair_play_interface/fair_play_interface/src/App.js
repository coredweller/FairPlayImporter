import React, { Component } from 'react';
import './App.css';
import Moment from 'moment';

class App extends Component {
  constructor(props) {
    super(props);
    this.state = {
      responsibilities: [],
      completedResponsibilities: []
    }
  }

  componentDidMount() {
    const url = "https://localhost:7207/Responsibility";
    fetch(url)
    .then(response => response.json())
    .then(json => this.setState({ responsibilities: json }))
  }

  onlyShowNonEmptyStr(label, valueStr) {
    if(valueStr) {
      return <p>{label}: {valueStr}</p>  
    }
    else {
      return "";
    }
  }

  completeResponsibility = (ev) =>{
    ev.preventDefault()
    console.log("YESS");
    //TODO: LEFT OFF HERE TRYING TO COMPLETE A RESPONSIBILITY ON BUTTON CLICK
      //Call API
      //If success then change background color or put it in a complete bucket at the bottom
      //Then when the page loads check to see if it has already been completed 

    const url = "https://localhost:7207/Responsibility";
    // fetch(url)
    // .then(response => response.json())
    // .then(json => this.setState({ completedResponsibilities: json }))
    // .then(res=> this.setState({quotes:[...this.state,res.data[0]]}))
  }

  render() {
    const { responsibilities } = this.state;
    
    return (
      <div class="container">
        <div class="jumbotron">
          <h1 class="display-4">Responsibilities</h1>
        </div>
        {responsibilities.map((resp, index) => {
          return (
            <div key={resp.date}>
              <div class="card__title">{Moment(resp.date).format('YYYY-MM-DD')}
              </div>
              {resp.responsibilities.map((r, index) => (
                <div key={r.playerTaskId}>
                  <div class="card">
                    <div class="card__title">
                      Id:{r.playerTaskId} - {r.cardName} - {r.taskType}
                    </div>
                    <div class="card__subtitle">
                      <p>Task #:{r.playerTaskId} - {r.cardName} - {r.taskType}</p>
                      <p>Cadence: {r.cadenceName}</p>
                      <p>{this.onlyShowNonEmptyStr("When", r.when)}</p>
                      <p>Requirement: {r.requirement}</p>
                      <p>{this.onlyShowNonEmptyStr("Minimum Standard", r.minimumStandard)}</p>
                      <p>{this.onlyShowNonEmptyStr("Notes", r.notes)}</p>
                    </div>
                    <div class="buttonsRightSide">
                      <button id="btnComplete" onClick={this.completeResponsibility} type="button">Complete</button>
                    </div>
                  </div>
                  
                </div>
              ))}
            </div>
          )
        })}
      </div>
    );
  }
}
export default App;