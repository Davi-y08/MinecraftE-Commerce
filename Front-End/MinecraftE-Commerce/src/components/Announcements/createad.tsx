import { useState } from "react";
import { useMutation } from "react-query";
import { useEffect } from "react";
import { data } from "react-router-dom";

function CreateAnnouncementPage() {
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [price, setPrice] = useState("");
  const [file, setFile] = useState(null);
  const [typeOfAnnouncement, setTypeOfAnnouncement] = useState('');
  const bearer = "Bearer " + localStorage.getItem("token");
  const [btnState, setBtnState] = useState('Enviar');
  const [responseState, setResponseState] = useState('');

  const onFileChange = async (e: any) => {
    setFile(e.target.files[0]);
  };

  async function createAdAsync(formData: FormData) {
    if (
      !(title == null && description == null && price == null && file == null && typeOfAnnouncement == null)
    ) {
      const response = await fetch("https://localhost:7253/api/v1/CreateAdd", {
        method: "POST",
        headers: {
          Authorization: bearer,
        },
        body: formData,
      })
      const data = await response.json();
      setResponseState(data.createdAdAction);
      console.log(setResponseState);

      return data;

    } else {
      alert("Preeencha todos os campos por favor!");
    }
    
  }

  const handleSubmit = (e: React.FormEvent) => {

    e.preventDefault();

    if (!title || !description || !price || !file || !typeOfAnnouncement) {
      alert("Por favor, preencha todos os campos.");
      return;
    }

    const formData = new FormData();
    formData.append("Title", title);
    formData.append("Description", description);
    formData.append("ImageAnnouncement", file);
    formData.append("PriceService", price);
    formData.append("TypeAnnouncement", typeOfAnnouncement);
    mutate(formData);
  };

  const {mutate, isLoading} = useMutation(createAdAsync, {
    onSuccess: (data) => {
        console.log(data);
    } 
  })


   useEffect(() => {
    if(isLoading){
      setBtnState("Carregando...");
    }
    else{
      setBtnState('Enviar');
    }
   });

  return (
    <div>
        <label htmlFor="titleforcreate">Title for announcemenet: </label>
        <input
          type="text"
          name="titleforcreate"
          placeholder="title"
          required
          onChange={(e) => setTitle(e.target.value)}
        />

        <br />
        <br />

        <label htmlFor="descriptionforcreate">Description for your ad: </label>
        <input
          type="text"
          name="descriptionforcreate"
          placeholder="Description"
          onChange={(e) => setDescription(e.target.value)}
          required
        />

        <br />
        <br />

        <label htmlFor="imageforcreate">Image for your announcement: </label>
        <input type="file" name="imageforcreate" onChange={onFileChange} required/>

        <br />
        <br />

        <label htmlFor="priceforcreate">Select your price ad: </label>
        <input
          type="number"
          name="priceforcreate"
          onChange={(e) => setPrice(e.target.value)}
          placeholder="Price"
          required
        />

        <br />
        <br />

        <label htmlFor="typeAnnouncement">Selecione o tipo de anúncio: </label>
        <select required onChange={(e) => setTypeOfAnnouncement(e.target.value)} name="typeAnnnouncement" id="typeOfAnnouncement">
          <option value="0">Plugin</option>
          <option value="1">Mod</option>
          <option value="2">Contrução</option>
          <option value="3">Serviço</option>
          <option value="4">Ajuda</option>
          <option value="5">Seeds</option>
          <option value="6">Skins</option>
        </select>

        <br />
        <br />

        <button onClick={handleSubmit}>
          {btnState}
        </button>

        {responseState && (
        <h2 style={{ color: 'green', display: 'block' }}>
          {responseState}
        </h2>
      )}
        
    </div>
  );
}

export default CreateAnnouncementPage;
