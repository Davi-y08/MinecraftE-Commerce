import { useEffect, useState } from "react";

export function SobreMim(){
    const token = localStorage.getItem('token');
    const [clicksInMounth, setClicksInMounth] = useState('');

    async function returnClicks() {
            const response = await fetch('https://localhost:7253/api/v1/cliquesem30dias', {
                method: 'GET',
                headers: {
                    "Authorization": "Bearer " + token,
                }
            })
    
            const data = await response.json();
            console.log(data);
            setClicksInMounth(data);
            console.log(clicksInMounth);
        }

        useEffect(() => {
            returnClicks();
        },[])

    return(
        <div>
            <p>{clicksInMounth}</p>
        </div>
    )
}

export function MinhasCompras(){
    return(
        <div>

        </div>
    )
}

export function MeusAnuncios(){
    return(
        <div>

        </div>
    )
}