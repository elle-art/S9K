import { Calendar, Event } from "./Calendar";

export interface S9KUser {
    displayName: string;
    invites: Invite[];
    tasks: Task[];
    weeklyGoal?: string;
}

export interface Invite {
    fromUser: string;
    message: string;
    event: Event;
}

export interface Task {
    name: string;
    date?: string;
    status: boolean;
}
