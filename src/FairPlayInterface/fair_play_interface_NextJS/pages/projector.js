import useSWR, { SWRConfig } from "swr";

const fetcher = (url) => fetch(url).then((res) => res.json());
const API = "https://127.0.0.1:49153/Responsibility";

export async function getServerSideProps() {
    const repoInfo = await fetcher(API);
    return {
      props: {
        fallback: {
          [API]: repoInfo
        }
      }
    };
  }

function Projector() {

    const { data, error } = useSWR(API);

    // there should be no `undefined` state
    console.log("Is data ready?", !!data);

    if (error) return "An error has occurred.";
    if (!data) return "Loading...";

    return (
        <ul>
            {data
            ? data.map((resp) => {
                return <li key={resp.playerTaskId}>{resp.requirement}</li>;
                })
            : null}
        </ul>
    );
}

export default function App({ fallback }) {
    return (
      <SWRConfig value={{ fallback }}>
        <Repo />
      </SWRConfig>
    );
  }