import React from "react";

const Textarea = (props) => {
  return (
    <div style={{ flex: 1 }}>
      <textarea
        className="p-2 border border-gray-300 rounded-md w-96 h-32 border-double "
        name="postContent"
        rows={4}
        cols={20}
        // placeholder="Voeg een opmerking toe"
        placeholder={props.text} 
      />
    </div>
  );
};

export default Textarea;
