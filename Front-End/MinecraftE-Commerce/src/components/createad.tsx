import { useState } from "react";

function CreateAnnouncementPage(){
    const [title, setTitle] = useState('');
    const [description, setDescription] = useState('');
    const [price, setPrice] = useState('');
    const [file, setFile] = useState<File | null>(null);
    const bearer = "Bearer " + localStorage.getItem('token');
    
    const onFileChange = async (e: any) => {
        setFile(e.target.files[0]);
    }

    const formData = new FormData();
    formData.append("image", file);
    formData.append("title", title);
    formData.append("description", description);
    formData.append("price", price);

    async function createAdAsync() {
        if (!(title == null && description == null && price == null && file == null)) {
            const response = await fetch('https://localhost:7253/api/v1/CreateAdd', {
                method: 'POST',
                headers: {
                    'Content-Type': 'multipart/form-data',
                    'Authorization': bearer
                },
                body: JSON.stringify({
                    
                })
            })
        }
        else{
            alert('erro na criação');
        }
    }

    return(
        <div>
            <label htmlFor="titleforcreate">Title for announcemenet: </label>
            <input type="text" name='titleforcreate' placeholder="title" onChange={(e) => setTitle(e.target.value)}/>

            <br /><br />

            <label htmlFor="descriptionforcreate">Description for your ad: </label>
            <input type="text" name="descriptionforcreate" placeholder="Description" onChange={(e) => setDescription(e.target.value)}/>

            <br /><br />

            <label htmlFor="imageforcreate">Image for your announcement: </label>
            <input type="file" name="imageforcreate" onChange={onFileChange}/>

            <br /><br />

            <label htmlFor="priceforcreate">Select your price ad: </label>
            <input type="number" name="priceforcreate" onChange={(e) => setPrice(e.target.value)} placeholder="Price"/>

            <br /><br />

            <button type="button">Create</button>
        </div>
    );
}

export default CreateAnnouncementPage;