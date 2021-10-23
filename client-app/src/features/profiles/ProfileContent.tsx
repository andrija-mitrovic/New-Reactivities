import { Tab } from "semantic-ui-react";
import { Profile } from "../../app/models/profile";
import ProfilePhoto from "./ProfilePhoto";
import ProfileFollowings from "./ProfileFollowings";
import ProfileActivities from "./ProfileActivities";
import { useStore } from "../../app/stores/store";

interface Props {
  profile: Profile;
}

export default function ProfileContent({ profile }: Props) {
  const { profileStore } = useStore();

  const panes = [
    { menuItem: "About", render: () => <Tab.Pane>About Content</Tab.Pane> },
    { menuItem: "Photos", render: () => <ProfilePhoto profile={profile} /> },
    { menuItem: "Events", render: () => <ProfileActivities /> },
    {
      menuItem: "Followers",
      render: () => <ProfileFollowings />,
    },
    {
      menuItem: "Following",
      render: () => <ProfileFollowings />,
    },
  ];
  return (
    <Tab
      menu={{ fluid: true, vertical: true }}
      menuPosition="right"
      panes={panes}
      onTabChange={(e, data) => profileStore.setActiveTab(data.activeIndex)}
    />
  );
}
