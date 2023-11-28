import { useState, ChangeEvent } from 'react';
import styles from './app.module.scss';
import Button from '@mui/material/Button';
import TextField from '@mui/material/TextField';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogContentText from '@mui/material/DialogContentText';
import DialogTitle from '@mui/material/DialogTitle';

interface SendResponsibilitiesEmailRequest {
    senderUserName: string;
    senderPassword: string;
    responsibilitySet: DailyResponsiblities
}

interface SendResponsibilitiesEmailResponse {
    isSuccess: boolean
}

export default function DailyDigestLink(props : {responsibilities: DailyResponsiblities, playerName: string}) {
    const [sendSuccessful, setSendSuccessful] = useState<SendResponsibilitiesEmailResponse>()
    const [modalOpen, setModalOpen] = useState(false);
    const [userEmail, setUserEmail] = useState('')
    const [userPassword, setUserPassword] = useState('')
    const [errorMessage, setErrorMessage] = useState('')

    function SendDailyDigest() 
    {
        //TODO: if email or password are empty then kick it back to user with error
        if(!userEmail || !userPassword) 
        {
            setErrorMessage("Please fill out all fields!");
            return;
        }

        const request: SendResponsibilitiesEmailRequest = {
            senderUserName: userEmail,
            senderPassword: userPassword,
            responsibilitySet: props.responsibilities
        }
        const url = "https://localhost:7207/Responsibility/send/digest/" + props.playerName;
        const requestOptions = {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(request)
          };
        fetch(url, requestOptions)
        .then(response => response.json())
        .then(json => setSendSuccessful(json))
        .then(() => setModalOpen(false));
    }

    function ShowSuccess() {
        if(sendSuccessful?.isSuccess) {
            return (
                <div>
                    <span className={styles.successText}>Email Sent Successfully!</span>
                </div>
            );
        }
        return "";
    }

    function ShowError() {
        if(errorMessage != '') {
            return (
                <div>
                    <br />
                    <span className={styles.failureText}>{errorMessage}</span>
                </div>
            );
        }
        return "";
    }

    const handleClose = () => {
        setModalOpen(false);
    };
    
    const handleClickOpen = () => {
        setModalOpen(true);
    };

    return (
        <div>
            <div onClick={handleClickOpen}>
                Send Daily Digest
            </div>
            <div>
                {ShowSuccess()}
            </div>
            <div>
                <Dialog open={modalOpen} onClose={handleClose}>
                    <DialogTitle>Send Daily Digest Email</DialogTitle>
                    <DialogContent>
                        <DialogContentText>
                            In order to use Gmail's free SMTP and NOT store your creds, please enter your Gmail email and password here.
                        </DialogContentText>
                        <div>
                            {ShowError()}
                        </div>
                        <TextField
                            autoFocus
                            margin="dense"
                            id="email"
                            label="Email Address"
                            type="email"
                            fullWidth
                            variant="standard"
                            onChange={(e: ChangeEvent<HTMLInputElement>) => {
                                setUserEmail(e.target.value)
                            }}
                        />
                        <TextField
                            label="Password"
                            id="password"
                            type="password"
                            onChange={(e: ChangeEvent<HTMLInputElement>) => {
                                setUserPassword(e.target.value)
                            }}>
                        </TextField>
                    </DialogContent>
                    <DialogActions>
                        <Button onClick={handleClose}>Cancel</Button>
                        <Button onClick={SendDailyDigest}>Send</Button>
                    </DialogActions>
                </Dialog>
            </div>
        </div>
    )
}
