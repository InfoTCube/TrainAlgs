import { TestGroup } from "./testGroup";

export interface Task {
    name: string;
    nameTag: string;
    contentUrl: string;
    authorUsername: string;
    memoryLimit: number;
    testGroups: TestGroup[];
}