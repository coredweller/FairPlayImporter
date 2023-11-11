import React, { Component } from 'react';

class App extends Component {
  constructor(props) {
    super(props);
    this.state = {
      responsibilities: []
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

  render() {
    const { responsibilities } = this.state;
    
    return (
      <div className="container">
        <div className="jumbotron">
          <h1 className="display-4">Responsibilities</h1>
        </div>
        {responsibilities.map((resp, index) => {
          return (
            <div className="card" key={resp.date}>
              <div className="card-header">{resp.date}
              </div>
              {resp.responsibilities.map((r, index) => (
                <div>
                  <div className="card-header">
                    #{index+1}
                  </div>
                  <div className="card-body">
                    <p className="card-text">Task #:{r.playerTaskId} - {r.cardName} - {r.taskType}</p>
                    <p>Cadence: {r.cadenceName}</p>
                    {this.onlyShowNonEmptyStr("When", r.when)}
                    <p>Requirement: {r.requirement}</p>
                    <p>{this.onlyShowNonEmptyStr("Minimum Standard", r.minimumStandard)}</p>
                    <p>{this.onlyShowNonEmptyStr("Notes", r.notes)}</p>
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