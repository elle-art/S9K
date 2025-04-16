import TimelineCalendarScreen from "@/frontend/utils/calendar/calendarProvider";
import React from "react";
import { SafeAreaView } from 'react-native';


export default function App() {

  return (
    <SafeAreaView style={{flex: 1}}>
      <TimelineCalendarScreen/>
    </SafeAreaView>
  );
}

