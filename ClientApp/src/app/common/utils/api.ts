import { isNullOrUndefined } from 'common/utils/common';

import CommandExecutionResult from 'models/response/commandExecutionResult';
import QueryExecutionResult from 'models/response/queryExecutionResult';

export function boxServerResponse(response: any): CommandExecutionResult {
    return (response
        ? ({
            success: false,
            error: response['Message'] || response.error,
        })
        : ({ success: true })) as CommandExecutionResult;
}

export function boxServerQueryResponse<TResult>(response: any): QueryExecutionResult<TResult> {
    return (isNullOrUndefined(response.Success)
        && (isNullOrUndefined(response.ok) || response.ok === true)
        ? ({
            success: true,
            result: response
        })
        : ({
            success: false,
            error: response['Message'] || response.error,
        })) as QueryExecutionResult<TResult>;
}