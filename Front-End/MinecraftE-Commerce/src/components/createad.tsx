import { useState } from "react";
import { useMutation } from "react-query";
import { data } from "react-router-dom";

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


  if (isLoading) {
    setBtnState('Carregando');
  }

  return (
    <div>
      <form onSubmit={handleSubmit}>
        <label htmlFor="titleforcreate">Title for announcemenet: </label>
        <input
          type="text"
          name="titleforcreate"
          placeholder="title"
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
        />

        <br />
        <br />

        <label htmlFor="imageforcreate">Image for your announcement: </label>
        <input type="file" name="imageforcreate" onChange={onFileChange} />

        <br />
        <br />

        <label htmlFor="priceforcreate">Select your price ad: </label>
        <input
          type="number"
          name="priceforcreate"
          onChange={(e) => setPrice(e.target.value)}
          placeholder="Price"
        />

        <br />
        <br />

        <button type="submit" value={"Submit"}>
          {btnState}
        </button>
      </form>
    </div>
  );
}

export default CreateAnnouncementPage;
