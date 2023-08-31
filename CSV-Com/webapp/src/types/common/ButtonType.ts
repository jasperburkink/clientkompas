export interface ButtonType {
    type: 'Solid' | 'NotSolid' | 'Underline'
}

export function getClassNameButtonType(param: string) {
    switch(param)
    {
      default:
      case 'Solid':
        return 'solid';
      case 'NotSolid':
        return 'not-solid';
      case 'Underline':
        return 'underline';
    }
  };