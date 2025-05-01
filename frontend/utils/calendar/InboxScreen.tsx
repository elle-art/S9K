import React, { useEffect, useState } from 'react';
import { View, Text, FlatList, Button, StyleSheet } from 'react-native';
import { getInboxInvites } from '@/frontend/hooks/getInboxInvites';

export default function InboxScreen() {
  const [invites, setInvites] = useState<any[]>([]);

  useEffect(() => {
    (async () => {
      const data = await getInboxInvites();
      setInvites(data);
    })();
  }, []);

  const handleAccept = (invite: any) => {
    // add invite.preConstructedEvent to user's calendar here
    // remove invite from inbox (you'll likely want a backend endpoint to do this)
    console.log('Accepted invite:', invite);
  };

  const handleDecline = (inviteId: string) => {
    console.log('Declined invite:', inviteId);
    // remove invite from inbox
  };

  return (
    <FlatList
      data={invites}
      keyExtractor={(item) => item.id}
      renderItem={({ item }) => (
        <View style={styles.card}>
          <Text style={styles.title}>{item.preConstructedEvent?.name}</Text>
          <Text>{item.message}</Text>
          <View style={styles.buttonRow}>
            <Button title="Accept" onPress={() => handleAccept(item)} />
            <Button title="Decline" color="red" onPress={() => handleDecline(item.id)} />
          </View>
        </View>
      )}
    />
  );
}

const styles = StyleSheet.create({
  card: {
    padding: 12,
    borderBottomWidth: 1,
    borderColor: '#ccc'
  },
  title: {
    fontWeight: 'bold',
    fontSize: 16
  },
  buttonRow: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginTop: 8
  }
});
