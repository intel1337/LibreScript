<script>
    import NewPost from "../Components/NewPost.svelte";
    import AuthGuard from "$lib/components/AuthGuard.svelte";
    import VerificationGuard from "$lib/components/VerificationGuard.svelte";
</script>

<AuthGuard>
    <VerificationGuard>
        <NewPost />
    </VerificationGuard>
</AuthGuard>