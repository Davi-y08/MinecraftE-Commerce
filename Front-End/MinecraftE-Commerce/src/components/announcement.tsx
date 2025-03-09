import axios from "axios";
import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import '../styles/card.css'
function AnnouncementPage(){

    interface Announcement{
        createdAt: string,
        descripton: string,
        id: number,
        imageAnnouncement: string,
        priceService: number,
        title: string,
        userId: string,
        userName: string,
        userPfp: string
    }

    const {id} = useParams();
    const [announcementAdd, setAnnouncement] = useState<Announcement>();

   async function returnAnn() {
        const response = await axios.get(`https://localhost:7253/api/v1/${id}`);
        const dataRes = response.data;
        if (response.status == 200) {
            setAnnouncement(dataRes);
            console.log(dataRes);
        }
   }    

   useEffect(() => {
    returnAnn();
   }, [])



    return(
        <div>
            {announcementAdd && (
                <div>
                    <h1>
                        {announcementAdd.title}
                    </h1>
                    <img width={100} src={`https://localhost:7253/${announcementAdd.imageAnnouncement}`}/>
                </div>
            )}        
        </div>
    )
}

export default AnnouncementPage;