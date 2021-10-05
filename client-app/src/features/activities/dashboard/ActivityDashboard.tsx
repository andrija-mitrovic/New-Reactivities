import { Grid, List } from "semantic-ui-react";
import { Activity } from "../../../app/models/activity";
import ActivityList from './ActivityList';

interface Props {
  activities: Activity[];
}

export default function ActivityDashboard({ activities }: Props) {
  return (
    <Grid>
      <Grid.Column width="10">
        <List>
          <ActivityList activities={activities} />
        </List>
      </Grid.Column>
      <Grid.Column width="6">

      </Grid.Column>
    </Grid>
  );
}