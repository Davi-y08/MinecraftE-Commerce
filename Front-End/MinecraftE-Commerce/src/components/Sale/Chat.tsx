import { useState } from "react";

function Chat(){
    const [userName, setUserName] = useState('');
    const [message, setMessage] = useState('');

    const sendMessage = async () => {
        try {
            await connection.invoke('SendMessage', {userName, message});
            setMessage('');
        } catch (error) {
            console.log(error);
        }
    }

    return(
        <div>
             <label className="lblName" htmlFor="name">Insira seu nome: </label>
        <input
         type="text" 
         id="name" 
         placeholder="Insira seu nome aqui"
         onChange={(e) => setUserName(e.target.value)}
         />

        <br />
        <br />

        <label className="lblMessage" htmlFor="message">Insira sua mensagem aqui: </label>
        <input 
        type="text" 
        id="message" 
        placeholder="Insira a mensagem aqui"
        onChange={(e) => setMessage(e.target.value)}
        />
        <button className="btnSend" onClick={sendMessage}>Enviar mensagem</button>
        </div>
    )
}

export default Chat;