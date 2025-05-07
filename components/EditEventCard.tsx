import React, { useState, useEffect } from 'react';
import {
  View,
  Modal,
  TextInput,
  StyleSheet,
  TouchableOpacity,
  Button
} from 'react-native';
import { Event, EVENT_TYPE_COLORS } from '@/frontend/constants/Calendar';

interface EventModalProps {
  visible: boolean;
  initialEvent?: Event;
  onSave: (event: Event) => void;
  onCancel: () => void;
}

export function EditEventCard({
  visible,
  initialEvent,
  onSave,
  onCancel
}: EventModalProps) {
  const [form, setForm] = useState<Event>(
    initialEvent || {
      name: '',
      date: '',
      time: { startTime: '', endTime: '' },
      type: '',
      group: []
    }
  );

  // Whenever initialEvent changes (e.g. opening for edit), reset the form
  useEffect(() => {
    if (initialEvent) setForm(initialEvent);
  }, [initialEvent]);

  const handleChange = <K extends keyof Event>(key: K, value: any) => {
    setForm((f) => ({ ...f, [key]: value } as Event));
  };

  const handleSave = () => {
    // you could add validation here
    onSave(form);
  };

  return (
    <Modal
      visible={visible}
      transparent
      animationType="slide"
      onRequestClose={onCancel}
    >
      <View style={styles.overlay}>
        <View style={styles.content}>
          <TextInput
            style={styles.input}
            placeholder="Event Name"
            value={form.name}
            onChangeText={(t) => handleChange('name', t)}
          />
          <TextInput
            style={styles.input}
            placeholder="Date (YYYY-MM-DD)"
            value={form.date}
            onChangeText={(t) => handleChange('date', t)}
          />
          <TextInput
            style={styles.input}
            placeholder="Start Time (HH:MM)"
            value={form.time.startTime}
            onChangeText={(t) =>
              handleChange('time', { ...form.time, startTime: t })
            }
          />
          <TextInput
            style={styles.input}
            placeholder="End Time (HH:MM)"
            value={form.time.endTime}
            onChangeText={(t) =>
              handleChange('time', { ...form.time, endTime: t })
            }
          />
          <TextInput
            style={styles.input}
            placeholder="Type"
            value={form.type}
            onChangeText={(t) => handleChange('type', t)}
          />
          <TextInput
            style={styles.input}
            placeholder="Group"
            value={form.group.join(', ')}
            onChangeText={(t) =>
              handleChange('group', t.split(',').map((s) => s.trim()))
            }
          />

          <View style={styles.buttons}>
            <Button title="Cancel" onPress={onCancel} />
            <Button title={initialEvent ? 'Update' : 'Create'} onPress={handleSave} />
          </View>
        </View>
      </View>
    </Modal>
  );
}

const styles = StyleSheet.create({
  overlay: {
    flex: 1,
    backgroundColor: '#00000080',
    justifyContent: 'center',
    padding: 20
  },
  content: {
    backgroundColor: '#fff',
    borderRadius: 8,
    padding: 16
  },
  input: {
    borderBottomWidth: 1,
    borderColor: '#ccc',
    marginVertical: 8,
    padding: 4
  },
  buttons: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginTop: 16
  }
});
