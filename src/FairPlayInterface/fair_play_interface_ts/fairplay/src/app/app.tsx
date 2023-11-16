// eslint-disable-next-line @typescript-eslint/no-unused-vars
import styles from './app.module.scss';
import Moment from 'moment';
import { useState, useEffect } from 'react';

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

function onlyShowNonEmptyStr(label: string, valueStr?: string) {
  if(valueStr) {
    return <div>{label}: {valueStr} </div>
  }
  else {
    return "";
  }
}

function completeResponsibility(bar: Responsibility)
{
  console.log(bar);
}

export function App() {
  const [age, setAge] = useState<number>(23)
  const [ responsibilities, setResponsibilities ] = useState<DailyResponsiblities[]>([]);

  useEffect(() => {
    //TODO: Get rid of hard coded URLS and add them to config
    const url = "https://localhost:7207/Responsibility";
    fetch(url)
    .then(response => response.json())
    .then(json => setResponsibilities(json))
  })

  return (
    <div className="container">
        <div className="jumbotron">
          <h1 className="display-4">Responsibilities</h1>
        </div>
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
