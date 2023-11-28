interface Responsibility {
    playerTaskId: bigint;
    cardName: string;
    suit: string;
    taskType: string;
    requirement: string;
    cadence: string;
    minimumStandard?: string;
    schedule?: string;
    when?: string;
    notes?: string;
}

interface DailyResponsiblities {
    date: Date,
    responsibilities: Responsibility[]
}
