import { useState } from "react";
import { useMutation } from "react-query";
import { useNavigate } from "react-router-dom";

function Login(){
    const [email, setEmail] = useState(""); 
    const [password, setPassword] = useState("");
    const [token, setToken] = useState('');
    const navigate = useNavigate();

    const fetchData = async ({ email, password }: { email: string, password: string }) => {
        const response = await fetch("https://localhost:7253/api/v1/Login", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization": "Bearer " + token,
            },
            body: JSON.stringify({
                'emailforlogin': email,
                'passwordforlogin': password
            }),
            credentials: 'include',
        });
        
    const data = await response.json();
    setToken(data.token);
    localStorage.setItem('token', data.token);
    localStorage.setItem('pfp', data.pfp);

    if (response.ok) {
        navigate('/');
    }  
}

    const handleSubmit = (e: any) => {
        e.preventDefault();
        mutate( {email, password} );
    }
    
    var stateBtn = "Send";

    const {mutate, isLoading} = useMutation(fetchData, {
        onSuccess: (data) => {
            console.log(data);
        }
    })

    if (isLoading) {
        stateBtn = "Loading";
    }

    
    return (
        <div>
            <form onSubmit={handleSubmit}>
                <label htmlFor="email">Email: </label>
                <input onChange={(e) => setEmail(e.target.value)} id="email" type="email" />
                <br /><br />
                <label htmlFor="password">Password: </label>
                <input onChange={(e) => setPassword(e.target.value)} type="password" />
                <br /> 
                <button type="submit" value={"Submit"}>{stateBtn}</button>
                </form>
        </div>
    )
}
export default Login;