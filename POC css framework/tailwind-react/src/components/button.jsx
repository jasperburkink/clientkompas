import '../index.css';

export function Button(props) {
  var type = props.type
  if(type === "NotSollid"){
    return (<button className={"btnNotSollid " +  (props.className)} href={props.href} onClick={props.onClick} type={props.typeOfBtn}>{props.text}</button>)
  }else{
    return (<button className={"btnSollid " +  (props.className)} href={props.href} onClick={props.onClick} type={props.typeOfBtn}>{props.text}</button>);
  }
}