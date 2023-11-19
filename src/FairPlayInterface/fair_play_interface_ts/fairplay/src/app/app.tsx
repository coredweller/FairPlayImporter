// eslint-disable-next-line @typescript-eslint/no-unused-vars
import styles from './app.module.scss';
import Moment from 'moment';
import { useState, useEffect, ChangeEvent } from 'react';
import PlayerChooser from './player_chooser';

interface DailyResponsiblities {
  date: Date,
  responsibilities: Responsibility[]
}

interface Responsibility {
  playerTaskId: bigint;
  cardName: string;
  suit: string;
  taskType: string;
  requirement: string;
  cadence: string;
  minimumStandard?: string;
  schedule?: string;
  when?: string;
  notes?: string;
}

interface CompleteResponsibilityRequest {
  playerTaskId: bigint;
  assignedDate: Date;
  completedDate: Date;
  notes: string;
}

function onlyShowNonEmptyStr(label: string, valueStr?: string) {
  if(valueStr) {
    return <div>{label}: {valueStr} </div>
  }
  else {
    return "";
  }
}

export function App() {
  const [ responsibilities, setResponsibilities ] = useState<DailyResponsiblities[]>([]);
  const [ playerName, setPlayerName ] = useState<string>("")
  const [ days, setDays ] = useState<number>(1)

  function playerNameChanged(props: ChangeEvent<HTMLInputElement>) {
    if(props.target.value.length < 3) return;

    console.log("SETTING PLAYERNAME FOUND!");
    setPlayerName(props.target.value);

    loadResponsibilities(props.target.value, days);
  }

  function daysChanged(props: ChangeEvent<HTMLInputElement>) {
    let latestDays = 1;
    try {
      let d = parseInt(props.target.value);
      if(d && d < 0){
        latestDays = d;
      }
    } catch (error) {
      console.log("error in daysChanged: " + error);
      latestDays = 1;
    }

    if(latestDays <= 0) return;

    console.log("SETTING DAYS DAYS DAYS DAYSSS!");
    setDays(latestDays);

    loadResponsibilities(playerName, latestDays);
  }

  function completeResponsibility(resp: Responsibility)
  {
      console.log(resp);
      const request: CompleteResponsibilityRequest = {
        playerTaskId: resp.playerTaskId,
        assignedDate: new Date(), //TODO: actually put the date the task is from here
        completedDate: new Date(),
        notes: '' //TODO: allow the user to add notes
      }
      const url = "https://localhost:7207/CompletedTask";
      const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(request) //TODO: this object is not what the API is expecting
      };
      
      fetch(url, requestOptions)
          .then(response => response.json())

      //TODO: make it so it doesn't bring back those that are already completed
      loadResponsibilities(playerName, days);
  }

  function loadResponsibilities(playerName: string, days: number){
    const url = "https://localhost:7207/Responsibility/" + playerName + "/" + days;
    fetch(url)
    .then(response => response.json())
    .then(json => setResponsibilities(json));
  }

  return (
    <div className="container">
        <div className="jumbotron">
          <h1 className="display-4">Responsibilities</h1>
        </div>
        <PlayerChooser playerName={playerName} playerNameChanged={playerNameChanged} days={days} daysChanged={daysChanged}></PlayerChooser>
        {responsibilities.map((resp, index) => {
          return (
            <div key={index}>
              <div className={styles.card__title}>{Moment(resp.date).format('YYYY-MM-DD')}
              </div>
              {resp.responsibilities.map((r, index) => (
                <div key={r.playerTaskId}>
                  <div className={styles.card}>
                    <div className={styles.card__title}>
                      Id:{r.playerTaskId.toString()} - {r.cardName} - {r.taskType}
                    </div>
                    <div className={styles.card__subtitle}>
                      <p>Task #:{r.playerTaskId.toString()} - {r.cardName} - {r.taskType}</p>
                      <p>Cadence: {r.cadence}</p>
                      <div>{onlyShowNonEmptyStr("When", r.when)}</div>
                      <p>Requirement: {r.requirement}</p>
                      <div>{onlyShowNonEmptyStr("Minimum Standard", r.minimumStandard)}</div>
                      <div>{onlyShowNonEmptyStr("Notes", r.notes)}</div>
                    </div>
                    <div className={styles.buttonsRightSide}>
                    {/* <button id="btnComplete" onClick={this.completeResponsibility} type="button">Complete</button> */}
                    <button id="btnComplete" onClick={() => completeResponsibility(r)} type="button">Complete</button>
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

export default App;
