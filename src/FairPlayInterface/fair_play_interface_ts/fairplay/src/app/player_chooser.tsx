import { ChangeEventHandler } from 'react';


export default function PlayerChooser(props : {playerName: string, days: number, playerNameChanged: ChangeEventHandler<HTMLInputElement>, daysChanged: ChangeEventHandler<HTMLInputElement>}) {

    return (
        <div>
            <input
                id="txtPlayerName"
                className="normal-input"
                type="text" 
                name="playerName" 
                placeholder="Name of Player"
                onChange={(pn) => props.playerNameChanged(pn)}/>
            <input
                id="txtDays"
                className="normal-input"
                type="text" 
                name="days" 
                placeholder="Days Projected"
                onChange={(d) => props.daysChanged(d)}/>
        </div>
    )
}
