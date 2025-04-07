// should hold API calls and interfaces for calendar-related objs/funcs
import groupBy from 'lodash/groupBy';
import filter from 'lodash/filter';
import find from 'lodash/find';

import React, {Component} from 'react';
import {Alert} from 'react-native';
import {
  ExpandableCalendar,
  TimelineEventProps,
  TimelineList,
  CalendarProvider,
  TimelineProps,
  CalendarUtils
} from 'react-native-calendars';

const getDate = (offset = 0) => {
  const date = new Date();
  date.setDate(date.getDate() + offset);
  return date.toISOString().split('T')[0];
};
// ***populate with user events form backend
const timelineEvents = [
  {
    id: '1',
    start: `${getDate()} 09:00:00`,
    end: `${getDate()} 10:00:00`,
    title: 'Meeting with team',
    color: 'blue'
  },
  {
    id: '2',
    start: `${getDate(1)} 14:00:00`,
    end: `${getDate(1)} 15:00:00`,
    title: 'Project brainstorming',
    color: 'green'
  }
];

const INITIAL_TIME = {hour: 9, minutes: 0};
const EVENTS: TimelineEventProps[] = timelineEvents;

// class can be found in react-native docs
export default class TimelineCalendarScreen extends Component {
  state = {
    currentDate: getDate(),
    events: EVENTS,
    eventsByDate: groupBy(EVENTS, e => CalendarUtils.getCalendarDateString(e.start)) as {
      [key: string]: TimelineEventProps[];
    },
    marked: { // ***change to mark all dates with an event
      [`${getDate(-1)}`]: {marked: true},
      [`${getDate()}`]: {marked: true},
      [`${getDate(1)}`]: {marked: true},
      [`${getDate(2)}`]: {marked: true},
      [`${getDate(4)}`]: {marked: true}
    }
  };

  onDateChanged = (date: string, source: string) => {
    console.log('TimelineCalendarScreen onDateChanged: ', date, source);
    this.setState({currentDate: date});
  };

  onMonthChange = (month: any, updateSource: any) => {
    console.log('TimelineCalendarScreen onMonthChange: ', month, updateSource);
  };
  // **edit to have popup
  createNewEvent: TimelineProps['onBackgroundLongPress'] = (timeString, timeObject) => {
    const {eventsByDate} = this.state;
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
        this.setState({eventsByDate});
      } else {
        eventsByDate[timeObject.date] = [newEvent];
        this.setState({eventsByDate: {...eventsByDate}});
      }
    }
  };
  // ***edit to have popup
  approveNewEvent: TimelineProps['onBackgroundLongPressOut'] = (_timeString, timeObject) => {
    const {eventsByDate} = this.state;

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
            const draftEvent = find(eventsByDate[timeObject.date], {id: 'draft'});
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
    unavailableHours: [{start: 0, end: 6}, {start: 22, end: 24}],
    overlapEventsSpacing: 8,
    rightEdgeSpacing: 24
  };

  render() {
    const {currentDate, eventsByDate} = this.state;

    return (
      <CalendarProvider
        date={currentDate}
        onDateChanged={this.onDateChanged}
        onMonthChange={this.onMonthChange}
        showTodayButton
        todayButtonStyle={{marginBottom: 25}}
        disabledOpacity={0.6}
      >
        <ExpandableCalendar
          firstDay={1}
          markedDates={this.state.marked}
        />
        <TimelineList
          events={eventsByDate}
          timelineProps={this.timelineProps}
          showNowIndicator
          scrollToNow
          initialTime={INITIAL_TIME}
        />
      </CalendarProvider>
    );
  }
}