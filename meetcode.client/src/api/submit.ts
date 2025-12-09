import { EnqueueResult } from "../types/enqueueResult";
import { RunCode } from "../types/runCode";
import { submitApi } from "./client"

export const runCode = async (runCodeRequest: RunCode): Promise<EnqueueResult> => {
    console.log(runCodeRequest);
    const response = await submitApi.post("/job/run", runCodeRequest);
    return response.data;
}