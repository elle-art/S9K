
import { useThemeColor } from "@/frontend/hooks/useThemeColor";
import { StyleSheet, View } from 'react-native';
import { ThemedText } from "./ThemedText";
import { Event, EVENT_TYPE_COLORS } from "@/frontend/constants/Calendar";

export function EventCard({ event }: { event: Event }) {

    return (
        <View style={styles.container}>

            {/* Card */}
            <View style={styles.card}>

                {/* Header */}
                <View style={styles.header}>
                    <ThemedText style={styles.title}>{event.name}</ThemedText>
                    {event.type && <ThemedText style={styles.subtitle}>{event.type}</ThemedText>}
                </View>

                <ThemedText style={styles.dateTime}>
                    {new Date(event.date).toLocaleDateString()} — {event.time.startTime} to {event.time.endTime}
                </ThemedText>

                <View style={styles.content}>
                    <ThemedText style={styles.text}>
                        Group: {event.group.join(', ')}
                    </ThemedText>
                </View>
            </View>
        </View>
    );
};

const styles = StyleSheet.create({
    container: {
        paddingVertical: 10,
        alignItems: 'center',
    },
    card: {
        backgroundColor: '#fff',
        borderRadius: 12,
        padding: 20,
        width: 340,
        shadowColor: '#000',
        shadowOffset: { width: 0, height: 2 },
        shadowOpacity: 0.15,
        shadowRadius: 4,
        elevation: 5,
    },
    header: {
        marginBottom: 10,
    },
    title: {
        fontSize: 22,
        fontWeight: '600',
        color: '#1a1a1a',
    },
    subtitle: {
        fontSize: 16,
        color: '#777',
        marginTop: 4,
    },
    dateTime: {
        fontSize: 15,
        color: '#555',
        marginVertical: 6,
    },
    content: {
        marginTop: 10,
    },
    text: {
        fontSize: 14,
        color: '#333',
        textAlign: 'center',
    },
});

