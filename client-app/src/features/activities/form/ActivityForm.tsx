import React from "react";
import { Button, Form, Segment } from "semantic-ui-react";

interface Props {
  closeForm: () => void;
}

export default function ActivityForm({closeForm}: Props) {
  return (
    <Segment clearing>
      <Form>
        <Form.Input placeholder="Title" />
        <Form.TextArea placeholder="Descripton" />
        <Form.Input placeholder="Category" />
        <Form.Input placeholder="Date" />
        <Form.Input placeholder="City" />
        <Form.Input placeholder="Venue" />
        <Button floated="right" positive type="submit" content="Submit" />
        <Button onClick={closeForm} floated="right" type="button" content="Cancel" />
      </Form>
    </Segment>
  );
}
