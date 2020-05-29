import CommandExecutionResult from './commandExecutionResult';

export default interface QueryExecutionResult<TResult> extends CommandExecutionResult {
    result?: TResult;
}