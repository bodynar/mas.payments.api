export default interface CommandExecutionResult {
    success: boolean;
    error?: string;
    args?: any;
}