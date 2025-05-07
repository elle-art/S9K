export interface Calendar {
    availability: TimeBlock[];
    preferredTimes: TimeBlock[];
    events: S9KEvent[];
}

export interface TimeBlock {
    startTime: string;
    endTime: string;
}

export interface S9KEvent {
    id?: string; 
    name: string;
    date: string;
    time: TimeBlock;
    type?: string;
    group: string[];
}

export const EVENT_TYPE_COLORS: Record<string, string> = {
    personal: 'lightblue',
    social: 'lightgreen',
    productive: 'plum',
    recreational: 'tomato',
    default: 'gray'
};
