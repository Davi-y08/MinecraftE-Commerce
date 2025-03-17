function CreateAnnouncementPage(){

    return(
        <div>
            <label htmlFor="titleforcreate">Title for announcemenet: </label>
            <input type="text" name='titleforcreate' placeholder="title"/>
            <br /><br />
            <label htmlFor="descriptionforcreate">Description for your ad: </label>
            <input type="text" name="descriptionforcreate"/>
            <br /><br />
        </div>
    );
}

export default CreateAnnouncementPage;