import React, { useState, useEffect } from 'react';
import {
    View,
    TextInput,
    StyleSheet,
    Button
} from 'react-native';
import { useUser } from '@/frontend/utils/user/userProvider';
import { ThemedText } from '@/components/ThemedText';
import { ThemedView } from '@/components/ThemedView';
import { S9KUser } from '@/frontend/constants/User';
import { buildFirebaseDataDoc } from '@/frontend/firebase-api/initializeUser';
import { getCalendarFromStorage } from '@/frontend/hooks/getCalendar';
import { getStoredUid } from '@/frontend/firebase-api/storageHelper';
import { persistUserData, saveDataToSessionStorage } from '@/frontend/firebase-api/initializeData';

export default function SettingsScreen() {
    const { user, setUser } = useUser();
    const [calendar, setCalendar] = useState<any>(null);
    const [form, setForm] = useState({
        displayName: user?.displayName ?? '',
        weeklyGoal: user?.weeklyGoal ?? 'schedule more!',
    });

    useEffect(() => {
        if (user?.displayName) setForm(f => ({ ...f, displayName: user.displayName }));
        if (user?.weeklyGoal) setForm(f => ({ ...f, weeklyGoal: user?.weeklyGoal ?? '' }));
    }, [user]);

    const handleChange = <K extends keyof typeof form>(key: K, value: any) =>
        setForm(f => ({ ...f, [key]: value }));

    const handleSave = async (name: string, goal: string) => {
        const data = await getCalendarFromStorage();
        const updatedUser: S9KUser = {
            ...user!,
            displayName: name,
            weeklyGoal: goal,
        };

        setUser(updatedUser);

        const firebaseData = buildFirebaseDataDoc(updatedUser, data);

        const storedUid = await getStoredUid();

        if (storedUid) {
            await persistUserData(storedUid, firebaseData);
        } else {
            console.error('No authenticated user found.');
        }

        await saveDataToSessionStorage(firebaseData);
        alert("All data saved successfully!");
    };

    return (
        <ThemedView style={styles.overlay}>
            <ThemedText type="title">Update profile</ThemedText>
            <View style={styles.content}>
                <ThemedText style={styles.label}>
                    Display Name
                </ThemedText>
                <TextInput
                    style={styles.input}
                    placeholder="Display Name"
                    value={form.displayName}
                    onChangeText={t => handleChange('displayName', t)}
                />

                <ThemedText style={styles.label}>
                    Weekly Goal
                </ThemedText>
                <TextInput
                    style={styles.input}
                    placeholder="e.g. complete 3 tasks"
                    value={form.weeklyGoal}
                    onChangeText={t => handleChange('weeklyGoal', t)}
                />

                <View style={styles.buttons}>
                    <Button title="Import Calendar" onPress={() => { }} />
                    <Button title="Save" onPress={() => handleSave(form.displayName, form.weeklyGoal)} />
                </View>
            </View>
        </ThemedView>
    );
}

const styles = StyleSheet.create({
    overlay: {
        flex: 1,
        justifyContent: 'center',
        padding: 20
    },
    content: {
        borderRadius: 8,
        padding: 16
    },
    label: {
        marginTop: 12,
        marginBottom: 4,
        fontWeight: '500'
    },
    input: {
        borderBottomWidth: 1,
        marginTop: 4,
        marginBottom: 12,
        padding: 4,
        backgroundColor: '#ffffff85',
        borderRadius: 4
    },
    buttons: {
        flexDirection: 'row',
        justifyContent: 'flex-start',
        gap: 12,
        marginTop: 16
    }
});
