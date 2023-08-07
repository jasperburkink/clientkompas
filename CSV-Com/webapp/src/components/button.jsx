import './button.scss';

export function Button(props) {
  return (
    <a className="btn" href={props.href}>{props.text}</a>
  );
}