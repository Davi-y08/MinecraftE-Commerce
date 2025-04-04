import { use, useState } from "react";
import { useMutation } from "react-query";
import { useEffect } from "react";

function CreateAnnouncementPage() {
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [price, setPrice] = useState("");
  const [file, setFile] = useState(null);
  const bearer = "Bearer " + localStorage.getItem("token");
  const [btnState, setBtnState] = useState('Enviar');

  const onFileChange = async (e: any) => {
    setFile(e.target.files[0]);
  };

  async function createAdAsync(formData: FormData) {
    if (
      !(title == null && description == null && price == null && file == null)
    ) {
      const response = await fetch("https://localhost:7253/api/v1/CreateAdd", {
        method: "POST",
        headers: {
          Authorization: bearer,
        },
        body: formData,
      });
      
      if(response.status == 200 || 201){
          const dataResponse = await response.json();
          console.log("resposta: " + dataResponse);
      }

    } else {
      alert("Preeencha todos os campos por favor!");
    }
  }

  const handleSubmit = (e: React.FormEvent) => {

    e.preventDefault();

    if (!title || !description || !price || !file) {
      alert("Por favor, preencha todos os campos.");
      return;
    }

    const formData = new FormData();
    formData.append("Title", title);
    formData.append("Description", description);
    formData.append("ImageAnnouncement", file);
    formData.append("PriceService", price);
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
   })

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

        <button onClick={handleSubmit}>
          {btnState}
        </button>
    </div>
  );
}

export default CreateAnnouncementPage;
