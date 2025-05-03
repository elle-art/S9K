// Interface for calendar provider used by index.tsx TimelineCalendarScreen component

import groupBy from 'lodash/groupBy';
import filter from 'lodash/filter';
import find from 'lodash/find';

import React, { Component } from 'react';
import { Alert, View } from 'react-native';
import {
  ExpandableCalendar,
  TimelineEventProps,
  TimelineList,
  CalendarProvider,
  TimelineProps,
  CalendarUtils
} from 'react-native-calendars';
import { getCalendarFromStorage } from '@/frontend/hooks/getCalendar';
import { Event, EVENT_TYPE_COLORS } from '@/frontend/constants/Calendar';
import { SafeAreaView, Image, ScrollView } from 'react-native';
import ParallaxScrollView from '@/components/ParallaxScrollView';
import { getEventsFromPhoneCalendar } from '@/frontend/hooks/getEventsFromFile';
import CalendarLogger from './frontend/hooks/calendarDataRetriever';
//import { getEventsFromPhoneCalendar } from '@/frontend/hooks/getEventsFromFile';
import { useEffect } from 'react';
import { retrieveAndStoreCalendarEvents } from '@/frontend/hooks/calendarDataRetriever';
import * as FileSystem from 'expo-file-system';


interface State {
  currentDate: string;
  events: TimelineEventProps[];
  eventsByDate: { [key: string]: TimelineEventProps[] };
  marked: { [key: string]: { marked: boolean } };
}

const getDate = (offset = 0) => {
  const date = new Date();
  date.setDate(date.getDate() + offset);
  return date.toISOString().split('T')[0];
};

const INITIAL_TIME = { hour: 9, minutes: 0 };

// class can be found in react-native docs
export default class TimelineCalendarScreen extends Component {
  state: State = {
    currentDate: getDate(),
    events: [],
    eventsByDate: {},
    marked: {}
  };

  async componentDidMount() {
    retrieveAndStoreCalendarEvents();
    await retrieveAndStoreCalendarEvents();
    console.log("yeet");
    const [storedCalendar, phoneEvents] = await Promise.all([
      getCalendarFromStorage(),
      getEventsFromPhoneCalendar()
    ]);

    const storedEvents: TimelineEventProps[] = storedCalendar?.events?.map((event: Event, index: number) => ({
      id: `stored-${index}`,
      start: `${event.date} ${event.time.startTime}`,
      end: `${event.date} ${event.time.endTime}`,
      title: event.name,
      color: EVENT_TYPE_COLORS[event.type ?? 'default']
    })) ?? [];

    const deviceEvents: TimelineEventProps[] = phoneEvents.map((event, index) => ({
      id: `device-${index}`,
      start: event.startDate,
      end: event.endDate,
      title: event.title || 'Untitled',
      color: '#ADD8E6' // light blue for phone events
    }));

    const allEvents = [...storedEvents, ...deviceEvents];

    const eventsByDate = groupBy(allEvents, e => CalendarUtils.getCalendarDateString(e.start));
    const marked = Object.keys(eventsByDate).reduce((acc, date) => {
      acc[date] = { marked: true };
      return acc;
    }, {} as State["marked"]);

    this.setState({
      currentDate: getDate(),
      events: allEvents,
      eventsByDate,
      marked
    });

    // Write the combined events to a file after data has been fetched
    const filePath = FileSystem.documentDirectory + 'events.json';
    await FileSystem.writeAsStringAsync(filePath, JSON.stringify(allEvents, null, 2));
    console.log("Events saved to file:", filePath);
  }



  onDateChanged = (date: string, source: string) => {
    console.log('TimelineCalendarScreen onDateChanged: ', date, source);
    this.setState({ currentDate: date });
  };

  onMonthChange = (month: any, updateSource: any) => {
    console.log('TimelineCalendarScreen onMonthChange: ', month, updateSource);
  };

  // **edit to have popup
  createNewEvent: TimelineProps['onBackgroundLongPress'] = (timeString, timeObject) => {
    const { eventsByDate } = this.state;
    const hourString = `${(timeObject.hour + 1).toString().padStart(2, '0')}`;
    const minutesString = `${timeObject.minutes.toString().padStart(2, '0')}`;

    const newEvent = {
      id: 'draft',
      start: `${timeString}`,
      end: `${timeObject.date} ${hourString}:${minutesString}:00`,
      title: 'New Event',
      color: 'white'
    };

    if (timeObject.date) {
      if (eventsByDate[timeObject.date]) {
        eventsByDate[timeObject.date] = [...eventsByDate[timeObject.date], newEvent];
        this.setState({ eventsByDate });
      } else {
        eventsByDate[timeObject.date] = [newEvent];
        this.setState({ eventsByDate: { ...eventsByDate } });
      }
    }
  };

  // ***edit to have popup
  approveNewEvent: TimelineProps['onBackgroundLongPressOut'] = (_timeString, timeObject) => {
    const { eventsByDate } = this.state;

    Alert.prompt('New Event', 'Enter event title', [
      {
        text: 'Cancel',
        onPress: () => {
          if (timeObject.date) {
            eventsByDate[timeObject.date] = filter(eventsByDate[timeObject.date], e => e.id !== 'draft');

            this.setState({
              eventsByDate
            });
          }
        }
      },
      {
        text: 'Create',
        onPress: eventTitle => {
          if (timeObject.date) {
            const draftEvent = find(eventsByDate[timeObject.date], { id: 'draft' });
            if (draftEvent) {
              draftEvent.id = undefined;
              draftEvent.title = eventTitle ?? 'New Event';
              draftEvent.color = 'lightgreen';
              eventsByDate[timeObject.date] = [...eventsByDate[timeObject.date]];

              this.setState({
                eventsByDate
              });
            }
          }
        }
      }
    ]);
  };

  private timelineProps: Partial<TimelineProps> = {
    format24h: true,
    onBackgroundLongPress: this.createNewEvent,
    onBackgroundLongPressOut: this.approveNewEvent,
    unavailableHours: [{ start: 0, end: 6 }, { start: 22, end: 24 }],
    overlapEventsSpacing: 8,
    rightEdgeSpacing: 24
  };

  render() {
    const { currentDate, eventsByDate } = this.state;

    return (
      <CalendarProvider
        date={currentDate}
        onDateChanged={this.onDateChanged}
        onMonthChange={this.onMonthChange}
        disabledOpacity={0.6}
      >
        <ScrollView contentContainerStyle={{ flexGrow: 1 }} nestedScrollEnabled={true}>
          <Image
            source={require('@/frontend/assets/images/partial-react-logo.png')}
            style={{ width: '100%', height: 200 }}
          />
          <ExpandableCalendar
            firstDay={1}
            markedDates={this.state.marked}
            scrollEnabled
          />
          <TimelineList
            events={eventsByDate}
            timelineProps={this.timelineProps}
            showNowIndicator
            scrollToNow
            initialTime={INITIAL_TIME}
          />
        </ScrollView>
      </CalendarProvider>
    );
  }
}

