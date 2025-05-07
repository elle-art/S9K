// TimelineCalendarScreen.tsx
import React, { useState, useEffect, useCallback } from 'react';
import { View, Button, ScrollView, Image } from 'react-native';
import {
  ExpandableCalendar,
  TimelineList,
  CalendarProvider,
  TimelineProps,
  CalendarUtils,
} from 'react-native-calendars';
import groupBy from 'lodash/groupBy';
import { getCalendarFromStorage } from '@/frontend/hooks/getCalendar';
import { Calendar, Event, EVENT_TYPE_COLORS } from '@/frontend/constants/Calendar';
import { EditEventCard } from '@/components/EditEventCard';
import { useUser } from '../user/userProvider';
import { saveDataToSessionStorage, persistUserData } from '@/frontend/firebase-api/initializeData';
import { buildFirebaseDataDoc } from '@/frontend/firebase-api/initializeUser';
import { getStoredUid } from '@/frontend/firebase-api/storageHelper';

const getDate = (offset = 0) => new Date(Date.now() + offset * 86400000).toISOString().split('T')[0];

export default function TimelineCalendarScreen() {
  const { user, setUser } = useUser();
  const [calendar, setCalendar] = useState<Calendar>();
  const [currentDate, setCurrentDate] = useState(getDate());
  const [eventsByDate, setEventsByDate] = useState<Record<string, any[]>>({});
  const [marked, setMarked] = useState<Record<string, { selected?: boolean; marked?: boolean }>>({});
  const [modalVisible, setModalVisible] = useState(false);
  const [editingEvent, setEditingEvent] = useState<Event | undefined>();
  const INITIAL_TIME = { hour: 9, minutes: 0 };

  const onDateChanged = useCallback((date: string, updateSource: string) => {
    console.log('Date changed to:', date, 'Source:', updateSource);
    setCurrentDate(date);

    setMarked(prevMarked => {
      const updatedMarked: typeof prevMarked = {};

      // Preserve existing markings
      for (const [dateKey, markProps] of Object.entries(prevMarked)) {
        updatedMarked[dateKey] = { ...markProps, selected: false };
      }

      // Add or update the selected date
      updatedMarked[date] = {
        ...(updatedMarked[date] || {}),
        selected: true,
      };

      return updatedMarked;
    });
  }, []);

  useEffect(() => {
    (async () => {
      const cal = await getCalendarFromStorage();
      setCalendar(cal);
      const evts = (cal?.events ?? []).map((e: Event, i: number) => ({
        id: `${i}`,
        start: `${e.date} ${e.time.startTime}`,
        end: `${e.date} ${e.time.endTime}`,
        title: e.name,
        color: EVENT_TYPE_COLORS[e.type ?? 'default'],
        userData: e
      }));
      const byDate = groupBy(evts, ev => CalendarUtils.getCalendarDateString(ev.start));
      setEventsByDate(byDate);
      setMarked(
        Object.keys(byDate).reduce((acc, d) => {
          acc[d] = { marked: true };
          return acc;
        }, {} as typeof marked)
      );

    })();
  }, []);

  const timelineProps: Partial<TimelineProps> = {
    format24h: true,
    onEventPress: ev => {
      const original: Event = (ev as any).userData as Event;
      setEditingEvent(original);
      setModalVisible(true);
    },
    unavailableHours: [{ start: 0, end: 6 }, { start: 22, end: 24 }],
    overlapEventsSpacing: 8,
    rightEdgeSpacing: 24,
  };

  const handleSaveEvent = async (evt: Event) => {
    const updatedEvents: Event[] = evt.id
      ? calendar!.events.map(e => e.id === evt.id ? evt : e)
      : [...(calendar?.events || []), evt];

    const updatedCalendar: Calendar = {
      ...calendar!,
      events: updatedEvents,
    };
    setCalendar(updatedCalendar);

    const day = evt.date;
    const timelineEvt = {
      id: evt.id ?? String(Date.now()),
      start: `${day} ${evt.time.startTime}`,
      end: `${day} ${evt.time.endTime}`,
      title: evt.name,
      color: EVENT_TYPE_COLORS[evt.type ?? 'default'],
      userData: evt
    };
    setEventsByDate(prev => ({
      ...prev,
      [day]: evt.id
        ? prev[day].map(e => e.id === timelineEvt.id ? timelineEvt : e)
        : [...(prev[day] || []), timelineEvt]
    }));

    const firebaseData = buildFirebaseDataDoc(user!, updatedCalendar!);

    const storedUid = await getStoredUid();

    if (storedUid) {
      await persistUserData(storedUid, firebaseData);
    } else {
      console.error('No authenticated user found.');
    }

    await saveDataToSessionStorage(firebaseData);

    setUser(user!);

    alert("All data saved successfully!");
    setModalVisible(false);
  };

  return (
    <CalendarProvider date={currentDate} onDateChanged={onDateChanged}>
      <ScrollView contentContainerStyle={{ flexGrow: 1 }} nestedScrollEnabled>
        {/* New Event button */}
        <View style={{ alignItems: 'flex-end', padding: 8 }}>
          <Button
            title="+ New Event"
            onPress={() => {
              setEditingEvent(undefined);
              setModalVisible(true);
            }}
          />
        </View>

        <Image
          source={require('@/frontend/assets/images/partial-react-logo.png')}
          style={{ width: '100%', height: 200 }}
        />

        <ExpandableCalendar firstDay={1} markedDates={marked} scrollEnabled />

        <TimelineList
          events={eventsByDate}
          timelineProps={timelineProps}
          showNowIndicator
          scrollToNow
          initialTime={INITIAL_TIME}
        />
        <EditEventCard
          visible={modalVisible}
          initialEvent={editingEvent}
          onCancel={() => setModalVisible(false)}
          onSave={handleSaveEvent}
        />
      </ScrollView>
    </CalendarProvider>
  );
}

